<script lang="ts">
  import { utilStore } from "$lib/UtilStore";
  import { themeStore } from "$lib/ThemeStore";
  import { userStore } from "$lib/userStore";
  import type { User } from "../Utils/customTypes";

  export let imgUrl: string;
  export let imgAlt: string;
  export let handleClick: () => void;

  let isDarkMode: boolean;
  let user: User;
  let isSmallScreen: boolean;
  let showProfileSidebar: boolean;
  $: isDarkMode = $themeStore.isDarkMode;
  $: user = $userStore;
  $: showProfileSidebar = $utilStore.showProfileSidebar;
  $: isSmallScreen = $utilStore.isSmallScreen;
</script>

<button on:click={handleClick} style={` background-color: transparent;`}>
  <img src={imgUrl} alt={imgAlt} />
</button>

<style>
  button {
    border: none;
    cursor: pointer;
    width: 28px;
    height: 28px;
  }
</style>
