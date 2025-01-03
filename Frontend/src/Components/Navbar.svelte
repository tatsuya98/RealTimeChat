<script lang="ts">
  import droneNeuro from "$lib/assests/droneNeuro.svg";
  import droneEvil from "$lib/assests/droneEvil.svg";
  import dark from "$lib/assests/dark-mode.svg";
  import light from "$lib/assests/light-mode.svg";
  import {
    handleAnchorStyleChange,
    handleButtonStyleChange,
    handleButtonStyleRevert,
    handleRevert,
    reconnectUser,
    toggleDarkMode,
    toggleDarkModeButtons,
    toggleSidebar,
  } from "../Utils/utils";
  import { themeStore } from "$lib/ThemeStore";
  import SearchBar from "./SearchBar.svelte";
  import type { User } from "../Utils/customTypes";
  import { userStore } from "$lib/userStore";
  import { connection } from "$lib/hubInstance";
  import isOnline from "is-online";
  import { utilStore } from "$lib/UtilStore";
  import ProfileSidebar from "./ProfileSidebar.svelte";
  import profileIcon from "$lib/assests/profile.svg";
  import IconButton from "./IconButton.svelte";
  let isDarkMode: boolean;
  const lightThemeColor = $themeStore.lightTheme["color-400"];
  const darkThemeColor = $themeStore.darkTheme["color-400"];
  const lightBackgroundColor = $themeStore.lightTheme["bg-400"];
  const darkBackgroundColor = $themeStore.darkTheme["bg-400"];
  let user: User;
  let isConnected: boolean;
  let showProfileSidebar: boolean;
  $: user = $userStore;
  $: isDarkMode = $themeStore.isDarkMode;
  $: isConnected = $utilStore.isConnected;
  $: showProfileSidebar = $utilStore.showProfileSidebar;

  connection.onclose(async () => {
    if (!(await isOnline())) {
      isConnected = false;
      utilStore.update(
        (state: {
          isOpen: boolean;
          showProfileSidebar: boolean;
          showSearchbar: boolean;
          showChatSidebar: boolean;
          isConnected: boolean;
          isSmallScreen: boolean;
        }) => {
          state.isConnected = false;
          return state;
        }
      );
    }
  });
</script>

<nav
  style="display: grid; align-items: center; justify-content: center;grid-template-columns: 0.3fr 1.2fr .1fr .1fr;"
>
  <ul style="padding: 0; list-style: none;">
    <li>
      <a
        href="/"
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
      >
        <img
          src={isDarkMode ? droneEvil : droneNeuro}
          alt="logo"
          width="306"
          height="51"
        />
      </a>
    </li>
  </ul>
  <SearchBar />
  <div style="position: relative;">
    {#if isConnected}
      <IconButton
        imgUrl={profileIcon}
        imgAlt="profile-icon"
        handleClick={() => toggleSidebar(showProfileSidebar, null)}
      />
      {#if showProfileSidebar}
        <ProfileSidebar />
      {/if}
    {/if}
  </div>

  <button
    class={``}
    style={`display: ${!isConnected ? "block" : "none"}`}
    on:click={() => {
      reconnectUser(user);
    }}
    on:mouseover={(e) =>
      handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
    on:focus={(e) =>
      handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
    on:blur={(e) =>
      handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
    on:mouseleave={(e) =>
      handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
    >Reconnect</button
  >

  <IconButton
    imgUrl={isDarkMode ? light : dark}
    imgAlt="toggle-theme"
    handleClick={() => {
      toggleDarkMode(
        isDarkMode,
        lightThemeColor,
        darkThemeColor,
        lightBackgroundColor,
        darkBackgroundColor
      );
    }}
  />
</nav>

<style>
  a {
    text-decoration: none;
    color: inherit;
    font-weight: bold;
  }
</style>
