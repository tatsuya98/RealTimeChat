<script lang="ts">
  import { goto } from "$app/navigation";
  import { userStore } from "$lib/userStore";
  import { onMount } from "svelte";
  import MessageWindow from "../../../Components/MessageWindow.svelte";
  import type { Message, User } from "../../../Utils/customTypes";
  import type { PageServerData } from "./$types";
  export let data: PageServerData;
  const messageHistory: Message[] = data.messages as Message[];
  let user: User;
  $: user = $userStore;
  onMount(() => {
    if (!user.isLoggedIn) {
      goto("/");
    }
  });
</script>

<MessageWindow
  documentId={data.id}
  messages={messageHistory}
  isDirectMessage={false}
  isGroupChat={true}
/>
