import { writable } from "svelte/store"
import type { User } from "../Utils/customTypes"

const initialState: User = {
    username: "",
    directMessageDocuments: [],
    groupChatDocuments: [],
    isLoggedIn: false,
    currentRecipient: ""
}
export const userStore = writable<User>(initialState);