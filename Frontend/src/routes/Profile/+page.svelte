<script lang="ts">
  import { userStore } from "$lib/userStore";
  import Button from "../../Components/Button.svelte";
  import type { User } from "../../Utils/customTypes";
  import closeIconLightMode from "$lib/assests/x-menu-light-mode.svg";
  import closeIconDarkMode from "$lib/assests/x-menu.svg";
  import { themeStore } from "$lib/ThemeStore";
  import { onMount } from "svelte";
  import { goto } from "$app/navigation";
  import { fade } from "svelte/transition";
  import { passwordMatchChek } from "../../Utils/utils";
  let user: User;
  let showModal: boolean;
  let inputPassword: string = "";
  let confirmPassword: string = "";
  let isDarkMode: boolean;
  let isSuccess: boolean = false;
  let isError: boolean = false;
  let message: string = "";
  const lightBackgroundColor = $themeStore.lightTheme["bg-400"];
  const darkBackgroundColor = $themeStore.darkTheme["bg-400"];
  $: user = $userStore;
  $: showModal = false;
  $: isDarkMode = $themeStore.isDarkMode;
  $: isSuccess = false;
  $: isError = false;
  onMount(() => {
    if (!user.isLoggedIn) {
      goto("/");
    }
  });

  function toggleModal() {
    showModal = !showModal;
  }
  async function updatePassword() {
    if (passwordMatchChek(inputPassword, confirmPassword)) {
      try {
        const response = await fetch(
          `https://localhost:7079/api/users/${user.username}`,
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({ password: inputPassword }),
          }
        );
        const data = await response.json();
        if (response.ok) {
          isSuccess = true;
          message = data.value.message;
          inputPassword = "";
          confirmPassword = "";
          toggleModal();
          setTimeout(() => {
            isSuccess = false;
          }, 3000);
        }
      } catch (err) {
        console.log(err);
      }
    } else {
      message = "Passwords don't match";
      isError = true;
      setTimeout(() => {
        isError = false;
      }, 3000);
    }
  }
</script>

{#if isSuccess}
  <div class="success" in:fade={{ duration: 500 }} out:fade={{ duration: 500 }}>
    <p>{message}</p>
  </div>
{/if}

<div style="display: flex;flex-direction: column;align-items: center;">
  <h1>{user.username}</h1>
  <div style="text-align: center; width:250px">
    <Button text="Change Password" handleClick={toggleModal} />
  </div>

  {#if showModal}
    <div class="modal">
      <div
        class="modal-content"
        style={`background-color: ${isDarkMode ? darkBackgroundColor : lightBackgroundColor}; position: relative;`}
      >
        {#if isError}
          <div
            class="error"
            in:fade={{ duration: 1000 }}
            out:fade={{ duration: 1000 }}
          >
            <p>{message}</p>
          </div>
        {/if}
        <button
          on:click={toggleModal}
          style={`position: absolute;top: 9%;right: 15%; cursor: pointer; background-color: ${isDarkMode ? darkBackgroundColor : lightBackgroundColor}; border: none;`}
        >
          <img
            src={isDarkMode ? closeIconDarkMode : closeIconLightMode}
            alt=""
            width="20"
            height="20"
          />
        </button>
        <div class="inputDiv">
          <label
            class="inputLabel"
            for="password"
            style={`${isDarkMode ? "color: white;" : "black"};`}
            >New Password</label
          >
          <input
            bind:value={inputPassword}
            type="password"
            id="password"
            name="password"
            required
          />
        </div>

        <div class="inputDiv">
          <label
            class="inputLabel"
            for="confirmPassword"
            style={`${isDarkMode ? "color: white;" : "black"};`}
            >Confirm Password</label
          >
          <input
            bind:value={confirmPassword}
            type="password"
            id="confirmPassword"
            name="confirmPassword"
          />
        </div>
        <div style="margin-block: 25px; margin-left: 17%">
          <Button text="Update Password" handleClick={updatePassword} />
        </div>
      </div>
    </div>
  {/if}
</div>

<style>
  .success,
  .error {
    margin-top: 45px;
    text-align: center;
    padding: 10px 35px;
    border-radius: 1rem;
  }
  .success {
    background-color: green;
  }
  .error {
    background-color: red;
  }
  .modal {
    display: block;
    position: fixed;
    z-index: 1;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
    background-color: rgb(0, 0, 0);
    background-color: rgba(0, 0, 0, 0.4);
  }

  .modal-content {
    background-color: #fefefe;
    margin: 15% auto;
    padding: 20px;
    border: 1px solid #888;
    width: 20%;
  }
  .inputLabel {
    font-size: 12px;
    color: #555;
    margin-bottom: 6px;
    margin-top: 24px;
  }
  .inputDiv {
    max-width: 250px;
    display: flex;
    flex-direction: column;
    margin: 10px auto;
  }
  input {
    height: 40px;
    font-size: 16px;
    border-radius: 4px;
    border: none;
    border: solid 1px #ccc;
    padding: 0 11px;
  }
  @media (max-width: 800px) {
    .modal-content {
      width: 250px;
    }
  }
  @media (max-width: 800px) {
    .success,
    .error {
      text-align: center;
      padding-inline: 35px;
      border-radius: 1rem;
    }
  }
</style>
