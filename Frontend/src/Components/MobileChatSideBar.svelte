<script lang="ts">
  import { themeStore } from "$lib/ThemeStore";
  import { userStore } from "$lib/userStore";
  import { slide } from "svelte/transition";
  import type { User } from "../Utils/customTypes";
  import closeIcon from "$lib/assests/x-menu.svg";
  import { toggleSidebar } from "../Utils/utils";
  import { utilStore } from "$lib/UtilStore";
  import IconButton from "./IconButton.svelte";
  let isDarkMode: boolean;
  let user: User;
  let showChatSidebar: boolean;
  $: isDarkMode = $themeStore.isDarkMode;
  $: user = $userStore;
  $: showChatSidebar = $utilStore.showChatSidebar;
</script>

<ul
  style="margin-top: 0"
  class="sidenav {isDarkMode ? 'sidenav-dark' : ''}"
  transition:slide={{ duration: 400, axis: "x" }}
>
  <IconButton
    imgUrl={closeIcon}
    imgAlt="close"
    handleClick={() => toggleSidebar(null, showChatSidebar)}
  />
  <div>
    <h3>Group Chats</h3>
    {#each user.groupChatDocuments as groupChat}
      <li>
        <a href="/GroupChat/{groupChat.DocumentId}"> {groupChat.RoomName}</a>
      </li>
    {/each}
  </div>
  <div>
    <h3>Direct Messages</h3>
    {#each user.directMessageDocuments as directMessage}
      <li>
        <a href="/DirectMessage/{directMessage.DocumentId}">
          {directMessage.Recipient}</a
        >
      </li>
    {/each}
  </div>
</ul>

<style>
  h3 {
    color: white;
  }
  ul {
    list-style-type: none;
  }
  li a {
    color: #818181;
  }
  .sidenav {
    height: 100%;
    width: 100%;
    position: fixed;
    z-index: 1;
    top: 0;
    left: 0;
    background-color: rgb(245, 205, 71);
    color: #818181;
    overflow-x: hidden;
    transition: 0.5s;
    padding-top: 60px;
    padding-left: 0;
    text-align: center;
  }
  .sidenav-dark {
    background-color: hsl(249, 10%, 13%);
  }
  .sidenav-dark h3 {
    color: white;
  }
  .sidenav a {
    padding-block: 0.5rem;
    text-decoration: none;
    font-size: 25px;
    width: fit-content;
    margin-inline: auto;
    display: block;
    transition: 0.3s;
  }
</style>
