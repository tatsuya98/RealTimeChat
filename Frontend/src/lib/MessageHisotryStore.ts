import { writable } from "svelte/store";

export const messageHistoryStore = writable(<string[]>[]);