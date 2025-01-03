import { writable } from "svelte/store";

export const utilStore = writable({
    isOpen: false,
    showProfileSidebar: false,
    showSearchbar: false,
    showChatSidebar: false,
    isConnected: true,
    isSmallScreen: false
});