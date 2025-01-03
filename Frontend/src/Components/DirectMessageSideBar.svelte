<script lang="ts">
  import { userStore } from "$lib/userStore";
  import type { User } from "../Utils/customTypes";
  import { goto } from "$app/navigation";
  import { resetMessagesReceived } from "../Utils/utils";
  import Button from "./Button.svelte";

  export let recipient: string;
  export let convoId: string;
  export let messagesReceived: number;
  let user: User = $userStore;
  $: user = user;
  $: messagesReceived = messagesReceived;
  function gotoToDirectMessageChat(): void {
    userStore.update((user) => {
      if (user) {
        user.currentRecipient = recipient;
      }
      return user;
    });
    resetMessagesReceived(
      convoId,
      user.username,
      "directMessage",
      messagesReceived
    );
    goto(`/DirectMessage/${convoId}`);
  }
</script>

<div class="direct-message-container" style="margin-left: 1rem;">
  <Button text={recipient} handleClick={gotoToDirectMessageChat} />

  <span>{messagesReceived}</span>
</div>
