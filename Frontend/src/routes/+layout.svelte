<script lang="ts">
  import { browser } from "$app/environment";
  import { userStore } from "$lib/userStore";
  import { onMount } from "svelte";
  import type { User } from "../Utils/customTypes";
  import { themeStore } from "$lib/ThemeStore";
  import Navbar from "../Components/Navbar.svelte";
  import MobileNavbar from "../Components/MobileNavbar.svelte";
  import { utilStore } from "$lib/UtilStore";

  let user: User;
  let isDarkMode: boolean = $themeStore.isDarkMode;
  let innerWidth: number = 0;
  let isSmallScreen: boolean = false;
  $: isSmallScreen = innerWidth <= 800;
  if (browser) {
    isDarkMode = JSON.parse(window.localStorage.getItem("darkMode") ?? "false");
  }
  onMount(() => {
    if (isDarkMode) {
      window.document.body.classList.toggle("dark-mode");
    }
  });
  $: user = $userStore;
  $: utilStore.update(
    (state: {
      isOpen: boolean;
      showProfileSidebar: boolean;
      showSearchbar: boolean;
      showChatSidebar: boolean;
      isConnected: boolean;
      isSmallScreen: boolean;
    }) => {
      state.isSmallScreen = isSmallScreen;
      return state;
    }
  );
</script>

<svelte:window bind:innerWidth />
<header>
  {#if isSmallScreen}
    <MobileNavbar />
  {:else}
    <Navbar />
  {/if}
</header>
<slot></slot>

<style>
  :global(body) {
    background-color: rgb(248, 230, 160);
    color: black;
    transition: background-color 0.3s;
  }
  :global(body.dark-mode) {
    background-color: hsl(0, 0%, 23%);
    color: white;
  }
  :global(.form-container) {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  :global(.form-input, .input-container) {
    display: flex;
    flex-direction: column;
  }
  :global(.input-container) {
    gap: 0.5rem;
    margin-bottom: 1rem;
  }
  :global(.form-input) {
    min-width: 300px;
  }
  :global(.dark-mode-nav a) {
    color: #818181;
  }
  :global(.dark-mode-nav h3) {
    color: #fff2f2;
  }
  :global(h1) {
    text-align: center;
  }
  :global(label) {
    font-weight: bold;
  }
  :global(.form-input input) {
    padding: 0.5rem;
    border: 1px solid black;
    border-radius: 4px;
  }
</style>
