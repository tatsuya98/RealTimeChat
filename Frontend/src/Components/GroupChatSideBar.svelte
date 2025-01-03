<script lang="ts">
  import { goto } from "$app/navigation";
  import { connection } from "$lib/hubInstance";
  import { userStore } from "$lib/userStore";
  import type { User } from "../Utils/customTypes";
  import { resetMessagesReceived } from "../Utils/utils";
  import Button from "./Button.svelte";
  export let roomName: string;
  export let groupChatId: string;
  export let messagesReceived: number;
  let user: User;
  $: messagesReceived = messagesReceived;
  $: user = $userStore;
  function goToGroupChat() {
    connection.invoke("AddUserToGroup", roomName);
    resetMessagesReceived(
      groupChatId,
      user.username,
      "groupChat",
      messagesReceived
    );
    goto(`/GroupChat/${groupChatId}`);
  }
</script>

<div class="group-chat-container" style="margin-left: 1rem;">
  <Button text={roomName} handleClick={goToGroupChat} />
  <span>{messagesReceived}</span>
</div>
