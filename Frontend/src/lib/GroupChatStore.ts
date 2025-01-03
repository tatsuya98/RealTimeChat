import { writable } from "svelte/store";
import type { GroupStore } from "../Utils/customTypes";
const initialState: GroupStore = {
    users: <string[]>[],
    isCreatingGroup: false
}
export const groupChatStore = writable(initialState);