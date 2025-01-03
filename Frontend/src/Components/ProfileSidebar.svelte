<script lang="ts">
  import { themeStore } from "$lib/ThemeStore";
  import { userStore } from "$lib/userStore";
  import { slide } from "svelte/transition";
  import type { User } from "../Utils/customTypes";
  import {
    handleAnchorStyleChange,
    handleRevert,
    Logout,
    toggleSidebar,
  } from "../Utils/utils";
  import { utilStore } from "$lib/UtilStore";
  const lightThemeColor = $themeStore.lightTheme["color-400"];
  const darkThemeColor = $themeStore.darkTheme["color-400"];
  let isDarkMode: boolean;
  let user: User;
  let isSmallScreen: boolean;
  let showProfileSidebar: boolean;
  let direction: "x" | "y" | undefined;
  $: isDarkMode = $themeStore.isDarkMode;
  $: user = $userStore;
  $: showProfileSidebar = $utilStore.showProfileSidebar;
  $: isSmallScreen = $utilStore.isSmallScreen;
  $: direction = isSmallScreen ? "x" : "y";
</script>

<ul
  style={`margin-top: 0px; padding: 25px; position: absolute; right: 35px; z-index: 100; top: 35px;`}
  class="sidenav {isDarkMode ? 'sidenav-dark' : ''}"
  transition:slide={{ duration: 400, axis: direction }}
>
  {#if user.isLoggedIn}
    <li><h3>{user.username}</h3></li>
    <li>
      <a
        href="/Profile"
        on:click={() => toggleSidebar(showProfileSidebar, null)}
      >
        Profile
      </a>
    </li>
    <li>
      <a
        href="/"
        on:click={() => {
          Logout();
          toggleSidebar(showProfileSidebar, null);
        }}>Logout</a
      >
    </li>
  {:else}
    <li>
      <a
        href="/Login"
        on:mouseover={(e) =>
          handleAnchorStyleChange(
            e,
            isDarkMode,
            lightThemeColor,
            darkThemeColor
          )}
        on:focus={(e) =>
          handleAnchorStyleChange(
            e,
            isDarkMode,
            lightThemeColor,
            darkThemeColor
          )}
        on:blur={(e) => handleRevert(e)}
        on:mouseleave={(e) => handleRevert(e)}
        on:click={() => toggleSidebar(showProfileSidebar, null)}>Login</a
      >
    </li>
    <li>
      <a
        href="/Register"
        on:mouseover={(e) =>
          handleAnchorStyleChange(
            e,
            isDarkMode,
            lightThemeColor,
            darkThemeColor
          )}
        on:focus={(e) =>
          handleAnchorStyleChange(
            e,
            isDarkMode,
            lightThemeColor,
            darkThemeColor
          )}
        on:blur={(e) => handleRevert(e)}
        on:mouseleave={(e) => handleRevert(e)}
        on:click={() => toggleSidebar(showProfileSidebar, null)}>Register</a
      >
    </li>
  {/if}
</ul>

<style>
  h3 {
    color: white;
  }
  ul {
    list-style-type: none;
  }
  a {
    text-decoration: none;
  }
  .sidenav {
    color: black;
    background-color: rgb(245, 205, 71);
  }
  .sidenav * {
    color: black;
  }
  .sidenav-dark {
    background-color: hsl(249, 10%, 13%);
  }
  .sidenav-dark h3 {
    color: white;
  }
  .sidenav-dark * {
    color: #818181;
  }
  .sidenav a {
    padding-block: 0.3rem;
    text-decoration: none;
    font-size: 1rem;
    color: #818181;
    width: fit-content;
    margin-inline: auto;
    margin-top: 1rem;
    display: block;
    transition: 0.3s;
  }
  @media (max-width: 800px) {
    .sidenav {
      height: 100%;
      width: 100%;
      position: fixed;
      z-index: 1;
      top: 0;
      right: 0;
      background-color: hsl(240, 6%, 12%);
      overflow-x: hidden;
      transition: 0.5s;
      padding-top: 60px;
      padding-left: 0;
      text-align: center;
    }
  }
</style>
