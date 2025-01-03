<script lang="ts">
  import { goto } from "$app/navigation";
  import { groupChatStore } from "$lib/GroupChatStore";
  import { connection } from "$lib/hubInstance";
  import { userStore } from "$lib/userStore";
  import { onMount } from "svelte";
  import type { GroupChat, GroupChatData } from "../../Utils/customTypes";
  import Button from "../../Components/Button.svelte";

  let groupChatName: string = "";
  let isPublic = true;
  const user = $userStore;
  let groupChatUsers: string[];
  let documentId: string;
  $: groupChatUsers = $groupChatStore.users;

  onMount(() => {
    groupChatStore.update((state) => {
      state.isCreatingGroup = true;
      return state;
    });
  });
  async function createGroupChat() {
    const dataToSend: GroupChatData = {
      RoomName: groupChatName,
      UsersToAdd: groupChatUsers,
      IsPublic: isPublic,
    };

    await connection.invoke("AddAllToGroup", dataToSend);
    connection.on("GroupChatCreated", (data: GroupChat) => {
      documentId = data.DocumentId;
      user?.groupChatDocuments?.push(data);
    });
    goto(`/GroupChat/${documentId}`);
  }
  function removeUser(username: string) {
    groupChatUsers = groupChatUsers.filter((user) => user !== username);
  }
</script>

<h1>Create Group</h1>
<div class="form-container">
  <div class="input-container">
    <div class="form-input">
      <label for="group-name">Group Chat Name</label>
      <input
        id="group-name"
        type="text"
        placeholder="Enter a group chat name"
        on:input={() => (groupChatName = groupChatName.trim())}
        bind:value={groupChatName}
        required
      />
    </div>
    <div style="display: flex; gap: 1rem;">
      <input
        type="checkbox"
        id="checkbox"
        on:click={() => {
          isPublic = !isPublic;
        }}
      />
      <label for="checkbox">Private</label>
    </div>
    <button on:click={createGroupChat}>Create Group</button>
  </div>
  <div class="added-users" style="display: flex; gap: .5rem;">
    {#each groupChatUsers as user}
      <p>{user}</p>
      <Button text="X" handleClick={() => removeUser(user)} />
    {/each}
  </div>
</div>
