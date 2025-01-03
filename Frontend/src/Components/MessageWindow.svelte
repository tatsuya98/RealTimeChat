<script lang="ts">
  import type {
    Message,
    recievedMessage,
    User,
    MessageDataToStore,
    GroupChat,
  } from "../Utils/customTypes";
  import { userStore } from "$lib/userStore";
  import { connection, setCallbacks } from "$lib/hubInstance";
  import * as signalR from "@microsoft/signalr";
  import { onMount } from "svelte";
  import {
    handleButtonStyleChange,
    handleButtonStyleRevert,
    handleGroupMessage,
    handlePrivateMessage,
    updateGeneralMessages,
  } from "../Utils/utils";
  import { themeStore } from "$lib/ThemeStore";
  import Sidebar from "./Sidebar.svelte";
  export let documentId: string = "";
  export let messages: Message[] = [];
  export let isDirectMessage: boolean;
  export let isGroupChat: boolean;

  let inputMessage: string = "";
  let user: User;
  let tempUsername: string = generateTempUsername();
  let isDarkMode: boolean;
  let darkThemeColor = $themeStore.darkTheme["color-400"];
  let lightThemeColor = $themeStore.lightTheme["color-400"];
  let lightBackgroundColor = $themeStore.lightTheme["bg-400"];
  let darkBackgroundColor = $themeStore.darkTheme["bg-400"];
  let innerWidth: number = 0;
  let isSmallScreen: boolean = false;
  $: isSmallScreen = innerWidth <= 800;
  $: user = $userStore;
  $: isDarkMode = $themeStore.isDarkMode;

  onMount(async () => {
    if (
      connection.state === signalR.HubConnectionState.Connected &&
      user?.isLoggedIn
    ) {
      connection.send("UpdateConnectedUsersId", user.username);
    }
  });

  setCallbacks({
    onReceiveMessage: (messageData: recievedMessage) => {
      messages = updateGeneralMessages(messages, messageData);
    },
    // update the message array for user who sent the message
    onSendMessageConfirmation(messageData: recievedMessage) {
      messages = updateGeneralMessages(messages, messageData);
    },
    onReceivePrivateMessage(messageData: recievedMessage) {
      messages = handlePrivateMessage(
        messages,
        messageData,
        documentId,
        isDirectMessage,
        user as User
      );
    },
    onGroupChatCreated(groupChatData: GroupChat) {
      user?.groupChatDocuments?.push(groupChatData);
      userStore.set(user);
    },
    onReceiveGroupChatMessage(messageData: recievedMessage) {
      messages = handleGroupMessage(
        messages,
        messageData,
        documentId,
        isGroupChat,
        user as User
      );
    },
  });

  function generateTempUsername(): string {
    const prefix = "Guest";
    const randomString = Math.random().toString(36).slice(2, 11);
    return `${prefix}_${randomString}`;
  }
  async function send(): Promise<void> {
    if (user?.username) {
      await connection.send("NewMessage", user.username, inputMessage);
    } else {
      await connection.send("NewMessageAnonymous", tempUsername, inputMessage);
    }
    inputMessage = "";
  }
  async function sendDirectMessage(): Promise<void> {
    if (user.currentRecipient.length > 0) {
      const messageDataToSend: MessageDataToStore = {
        Content: inputMessage,
        Sender: user.username as string,
        Recipient: user.currentRecipient,
      };
      await connection.send(
        "SendPrivateMessage",
        messageDataToSend,
        documentId
      );
    }
    inputMessage = "";
  }

  async function sendGroupChatMessage(): Promise<void> {
    const groupChatName = user?.groupChatDocuments?.find(
      (doc) => doc.DocumentId === documentId
    )?.RoomName;
    const messageDataToSend: Message = {
      Content: inputMessage,
      Sender: user?.username as string,
    };
    await connection.send(
      "SendGroupMessage",
      messageDataToSend,
      documentId,
      groupChatName as string
    );
  }
  function handleSendFunctionCall() {
    if (isDirectMessage) {
      sendDirectMessage();
    } else if (isGroupChat) {
      sendGroupChatMessage();
    } else {
      send();
    }
    inputMessage = "";
  }
</script>

<svelte:window bind:innerWidth />
<main>
  <div class="grid-container">
    {#if !isSmallScreen}
      <Sidebar />
    {/if}
    <div class="message-window">
      <div
        class="messages"
        style={`background-color: ${
          isDarkMode ? darkBackgroundColor : lightBackgroundColor
        }`}
      >
        {#each messages as message}
          <div class="message-container" style="margin-left: 1rem;">
            <p style="font-weight: bold;">{message.Sender}:</p>
            <p>{message.Content}</p>
          </div>
        {/each}
      </div>
      <div class="message-input" style="display: flex;">
        <input
          class="message-input"
          style={`width: 100%; background-color: ${isDarkMode ? "hsl(249, 10%, 13%)" : "white"}; color: ${isDarkMode ? "white" : "black"}; border-radius: 4px 0 0 4px;`}
          type="text"
          placeholder="Type a message"
          bind:value={inputMessage}
          on:keyup={(e) => e.key === "Enter" && handleSendFunctionCall()}
        />
        <button
          on:mouseover={(e) =>
            handleButtonStyleChange(
              e,
              isDarkMode,
              lightThemeColor,
              darkThemeColor
            )}
          on:focus={(e) =>
            handleButtonStyleChange(
              e,
              isDarkMode,
              lightThemeColor,
              darkThemeColor
            )}
          on:blur={(e) =>
            handleButtonStyleRevert(
              e,
              isDarkMode,
              lightThemeColor,
              darkThemeColor
            )}
          on:mouseleave={(e) =>
            handleButtonStyleRevert(
              e,
              isDarkMode,
              lightThemeColor,
              darkThemeColor
            )}
          style={`background-color: ${isDarkMode ? darkThemeColor : lightThemeColor}; font-weight: bold;`}
          on:click={handleSendFunctionCall}>Send</button
        >
      </div>
    </div>
  </div>
</main>

<style>
  @media (min-width: 800px) {
    .grid-container {
      display: grid;
      grid-template-columns: 200px 85vw;
    }
  }
  .messages {
    position: relative;
    min-height: 85vh;
  }
  .message-input {
    position: sticky;
    bottom: 0;
    border-style: none;
    border-radius: 4px;
  }
  button {
    background-color: hsl(181, 100%, 27%);
    color: white;
    border: none;
    border-radius: 4px;
    padding: 0.5rem;
    text-transform: uppercase;
  }
  button:hover {
    cursor: pointer;
  }
</style>
