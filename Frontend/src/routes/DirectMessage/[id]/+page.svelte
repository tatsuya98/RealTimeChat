<script lang="ts">
  import { userStore } from "$lib/userStore";
  import { onMount } from "svelte";
  import MessageWindow from "../../../Components/MessageWindow.svelte";
  import type { Message, User } from "../../../Utils/customTypes";
  import type { PageServerData } from "./$types";
  import { goto } from "$app/navigation";

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

<div>
  <MessageWindow
    documentId={data.id}
    messages={messageHistory}
    isDirectMessage={true}
    isGroupChat={false}
  />
</div>
