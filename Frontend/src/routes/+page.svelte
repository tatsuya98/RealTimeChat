<script lang="ts">
  import { userStore } from "$lib/userStore";
  import MessageWindow from "../Components/MessageWindow.svelte";
  import type { User } from "../Utils/customTypes";
  let user: User;
  $: user = $userStore;
</script>

<MessageWindow documentId="" isDirectMessage={false} isGroupChat={false} />
