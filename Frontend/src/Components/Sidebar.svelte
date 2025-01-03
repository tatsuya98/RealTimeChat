<script lang="ts">
  import { themeStore } from "$lib/ThemeStore";
  import { userStore } from "$lib/userStore";
  import type { User } from "../Utils/customTypes";
  import DirectMessageSideBar from "./DirectMessageSideBar.svelte";
  import GroupChatSideBar from "./GroupChatSideBar.svelte";
  let user: User;
  let isDarkMode: boolean;
  $: user = $userStore;
  $: isDarkMode = $themeStore.isDarkMode;
</script>

<div
  class="sidebar"
  style={`background-color: ${isDarkMode ? "hsl(249, 10%, 13%)" : "rgb(245,205,71)"}`}
>
  <div class="group-chat">
    <h3>Group Chats</h3>
    {#if user && user.groupChatDocuments && user.groupChatDocuments.length > 0 && user.isLoggedIn}
      {#each user.groupChatDocuments as groupChat}
        <GroupChatSideBar
          roomName={groupChat.RoomName}
          groupChatId={groupChat.DocumentId}
          messagesReceived={groupChat.MessagesReceived}
        />
      {/each}
    {/if}
  </div>
  <div class="direct-message">
    <h3>Direct Messages</h3>
    {#if user.directMessageDocuments && user.directMessageDocuments.length > 0 && user.isLoggedIn}
      {#each user.directMessageDocuments as directMessage}
        <DirectMessageSideBar
          recipient={directMessage.Recipient}
          convoId={directMessage.DocumentId}
          messagesReceived={directMessage.MessagesReceived}
        />
      {/each}
    {/if}
  </div>
</div>

<style>
  h3 {
    margin-left: 1rem;
  }
</style>
