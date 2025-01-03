<script lang="ts">
  import droneNeuroMobile from "$lib/assests/droneNeuroMobile.svg";
  import droneEvilMobile from "$lib/assests/droneEvilMobile.svg";
  import menu from "$lib/assests/menu.svg";
  import dark from "$lib/assests/dark-mode.svg";
  import light from "$lib/assests/light-mode.svg";
  import { utilStore } from "$lib/UtilStore";
  import { themeStore } from "$lib/ThemeStore";
  import profile from "$lib/assests/profile.svg";
  import {
    handleAnchorStyleChange,
    handleRevert,
    toggleDarkMode,
    toggleSearchbar,
    toggleSidebar,
  } from "../Utils/utils";
  import SearchBar from "./SearchBar.svelte";
  import MobileChatSideBar from "./MobileChatSideBar.svelte";
  import type { User } from "../Utils/customTypes";
  import { userStore } from "$lib/userStore";
  import ProfileSidebarMobile from "./ProfileSidebarMobile.svelte";
  import searchIconLight from "$lib/assests/search-light-mode.svg";
  import IconButton from "./IconButton.svelte";
  let open: boolean;
  let showProfileSidebar: boolean;
  let isDarkMode: boolean;
  const lightThemeColor = $themeStore.lightTheme["color-400"];
  const darkThemeColor = $themeStore.darkTheme["color-400"];
  const lightBackgroundColor = $themeStore.lightTheme["bg-400"];
  const darkBackgroundColor = $themeStore.darkTheme["bg-400"];
  let showChatSidebar: boolean;
  let user: User;
  let showSearchbar: boolean;
  $: user = $userStore;
  $: showChatSidebar = $utilStore.showChatSidebar;
  $: open = $utilStore.isOpen;
  $: isDarkMode = $themeStore.isDarkMode;
  $: showProfileSidebar = $utilStore.showProfileSidebar;
  $: showSearchbar = $utilStore.showSearchbar;
</script>

<nav style=" margin-top: 2rem;">
  <ul
    style="display: flex;align-items: center;justify-content: space-between; padding: 0 10px;"
  >
    {#if !open && user.isLoggedIn && (user.directMessageDocuments.length > 0 || user.groupChatDocuments.length > 0)}
      <li>
        <IconButton
          imgUrl={menu}
          imgAlt="menu"
          handleClick={() => toggleSidebar(null, showChatSidebar)}
        />
      </li>
    {/if}
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
          src={isDarkMode ? droneEvilMobile : droneNeuroMobile}
          alt="logo"
          width="44"
          height="44"
        />
      </a>
    </li>
    <li>
      <IconButton
        imgUrl={searchIconLight}
        imgAlt="search"
        handleClick={() => toggleSearchbar(showSearchbar)}
      />
    </li>
    <li>
      <IconButton
        imgUrl={profile}
        imgAlt="profile"
        handleClick={() => toggleSidebar(showProfileSidebar, null)}
      />
    </li>
    <li>
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
    </li>
  </ul>
</nav>
{#if showProfileSidebar}
  <ProfileSidebarMobile />
{/if}
{#if showChatSidebar}
  <MobileChatSideBar />
{/if}
{#if showSearchbar}
  <div style="text-align: center;">
    <SearchBar />
  </div>
{/if}

<style>
  ul {
    list-style-type: none;
  }
  li {
    margin-left: 1rem;
  }
</style>
