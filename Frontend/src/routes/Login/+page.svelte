<script lang="ts">
  // process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
  import { enhance } from "$app/forms";
  import { goto } from "$app/navigation";
  import { themeStore } from "$lib/ThemeStore";
  import { userStore } from "$lib/userStore";
  import { fade } from "svelte/transition";
  import type { User } from "../../Utils/customTypes";
  import {
    convertUnknownDataToUserData,
    convertUnknownErrorMessageToString,
    handleButtonStyleChange,
    handleButtonStyleRevert,
    hideError,
  } from "../../Utils/utils";

  let user: User;
  let errorMessage: string | null;
  let isDarkMode: boolean;
  let lightThemeColor = $themeStore.lightTheme["color-400"];
  let darkThemeColor = $themeStore.darkTheme["color-400"];

  $: isDarkMode = $themeStore.isDarkMode;
  $: errorMessage = "";
</script>

<div class="form-container">
  {#if errorMessage}
    <div
      class="error"
      in:fade={{ duration: 1000 }}
      out:fade={{ duration: 1000 }}
    >
      <p>{errorMessage}</p>
    </div>
  {/if}
  <h1>Login</h1>
  <form
    class="form-input"
    method="POST"
    use:enhance={() => {
      return async ({ result }) => {
        if (result.type === "success" && result.data) {
          user = convertUnknownDataToUserData(result.data.userDataObject);

          userStore.set(user);
          goto("/");
        } else if (result.type === "failure" && result.data) {
          errorMessage = convertUnknownErrorMessageToString(result.data.value);
          setTimeout(() => {
            errorMessage = null;
          }, 2500);
        }
      };
    }}
  >
    <div class="input-container">
      <label for="username">Username</label>
      <input
        on:focus={() => {
          hideError(errorMessage);
        }}
        id="username"
        type="text"
        name="username"
        placeholder="Enter username"
        required
      />
    </div>
    <div class="input-container">
      <label for="password">Password</label>
      <input
        on:focus={() => {
          hideError(errorMessage);
        }}
        id="password"
        type="password"
        name="password"
        placeholder="Enter password"
        required
      />
    </div>
    <button
      class={isDarkMode ? "dark-Button-mode" : ""}
      on:mouseover={(e) =>
        handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
      on:focus={(e) =>
        handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
      on:blur={(e) =>
        handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
      on:mouseleave={(e) =>
        handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
      type="submit">Login</button
    >
  </form>
</div>

<style>
  .error {
    background-color: red;
    padding: 10px 35px;
    border-radius: 1rem;
  }
  button {
    background-color: hsl(181, 100%, 27%);
    color: white;
    border: none;
    border-radius: 4px;
    padding: 0.5rem;
    text-transform: uppercase;
  }
  button.dark-Button-mode {
    background-color: hsl(341, 100%, 36%);
  }
</style>
