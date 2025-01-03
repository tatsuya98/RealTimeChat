<script lang="ts">
  import { themeStore } from "$lib/ThemeStore";
  import {
    handleButtonStyleChange,
    handleButtonStyleRevert,
  } from "../Utils/utils";

  export let text: string;
  export let handleClick: () => void;
  let isDarkMode: boolean;
  let lightThemeColor = $themeStore.lightTheme["color-400"];
  let darkThemeColor = $themeStore.darkTheme["color-400"];
  let button: HTMLButtonElement;
  $: isDarkMode = $themeStore.isDarkMode;
</script>

<button
  class={`button ${isDarkMode ? "dark-Button-mode" : ""}`}
  style="font-weight: bold;"
  bind:this={button}
  on:click={handleClick}
  on:mouseover={(e) =>
    handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
  on:focus={(e) =>
    handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
  on:blur={(e) =>
    handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
  on:mouseleave={(e) =>
    handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
  >{text}</button
>

<style>
  button {
    background-color: hsl(181, 100%, 27%);
    color: white;
    border: none;
    border-radius: 4px;
    padding: 0.5rem;
    text-transform: uppercase;
    width: 80%;
  }
  button.dark-Button-mode {
    background-color: hsl(341, 100%, 36%);
  }
</style>
