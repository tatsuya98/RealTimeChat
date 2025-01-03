<script lang="ts">
  import { goto } from "$app/navigation";
  import { groupChatStore } from "$lib/GroupChatStore";
  import { themeStore } from "$lib/ThemeStore";
  import { userStore } from "$lib/userStore";
  import type { DirectMessage, SearchedUser, User } from "../Utils/customTypes";
  import { generateRandomString } from "../Utils/utils";
  import Button from "./Button.svelte";
  let inputValue: string = "";
  let foundUsers: SearchedUser[] = [];
  let showResults: boolean = false;
  let timeoutId: number | undefined | NodeJS.Timeout;
  let user: User;
  let isDarkMode: boolean;
  const usersToAdd: string[] = $groupChatStore.users;
  $: user = $userStore;
  $: isDarkMode = $themeStore.isDarkMode;
  const randomStringLength = 8;
  async function searchUsers(searchValue: string) {
    if (searchValue.length > 0) {
      try {
        const response = await fetch(
          `https://localhost:7079/api/users/${searchValue}`
        );
        const data = await response.json();
        foundUsers = [data];
        showResults = true;
      } catch (err) {
        console.log(err);
      }
    }
  }
  function setCurrentRecipient(e: Event, recipient: string) {
    e.preventDefault();
    userStore.update((user) => {
      if (user) {
        user.currentRecipient = recipient;
      }
      return user;
    });
  }
  function addUser(foundUser: SearchedUser): void {
    usersToAdd.push(foundUser.username);
    groupChatStore.update((state) => {
      state.users = usersToAdd;
      return state;
    });
    inputValue = "";
    showResults = false;
  }
  function checkuserDirectMessageDocuments(username: string): DirectMessage {
    if (user.directMessageDocuments.some((doc) => doc.Recipient === username)) {
      return user.directMessageDocuments.find(
        (doc) => doc.Recipient === username
      )!;
    }
    return {
      DocumentId: "",
      Recipient: "",
      MessagesReceived: 0,
    };
  }
  function gotoNewPage(username: string): void {
    const result: DirectMessage = checkuserDirectMessageDocuments(username);
    if (result.DocumentId.length > 0) {
      goto(`/DirectMessage/${result.DocumentId}`);
    } else {
      goto(`/DirectMessage/${generateRandomString(randomStringLength)}`);
    }
  }

  $: {
    if (inputValue.length === 0) {
      foundUsers = [];
      showResults = false;
    }
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => {
      if (inputValue.length > 0) {
        searchUsers(inputValue);
      }
    }, 1000);
  }
</script>

<div
  class="search-container"
  style="flex-grow: 1;margin-left: 1rem; position: relative;"
>
  <div>
    <input
      class="search-bar"
      style={`background-color: ${isDarkMode ? "hsl(249, 11%, 13%)" : "white"};
      color: ${isDarkMode ? "white" : "black"};`}
      type="text"
      name=""
      id=""
      placeholder="Search"
      on:input={() => (inputValue = inputValue.trim())}
      bind:value={inputValue}
    />
  </div>
  {#if showResults}
    <div class="search-results">
      {#each foundUsers as foundUser}
        <div class="info-card">
          <p>{foundUser.username}</p>
          {#if $groupChatStore.isCreatingGroup}
            <button
              on:click={() => {
                addUser(foundUser);
              }}>Add</button
            >
          {/if}
          {#if user?.isLoggedIn && user.username !== foundUser.username}
            <a
              href="#message"
              on:click={(e) => {
                setCurrentRecipient(e, foundUser.username);
                inputValue = "";
                showResults = false;
                foundUsers = [];
                gotoNewPage(foundUser.username);
              }}><Button text="Message" handleClick={() => {}} /></a
            >
          {/if}
        </div>
      {/each}
    </div>
  {/if}
</div>

<style>
  .search-bar {
    width: calc(100% - 6rem);
    border-style: none;
    border-radius: 4px;
    padding: 0.5rem;
  }
  .search-results {
    position: absolute;
    padding: 1rem 1.3rem;
    border-radius: 4px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.75);
    z-index: 2;
  }
</style>
