import { writable } from "svelte/store";

const isDarkMode: boolean = false;
const lightTheme = {
    "bg-400": "rgb(248, 230, 160)",
    "color-400": "hsl(181, 100%, 27%)"
}
const darkTheme = {
    "bg-400": "hsl(0, 0%, 23%)",
    "color-400": "hsl(341, 100%, 36%)"
}
const themestyle = {
    isDarkMode,
    lightTheme, darkTheme
}
export const themeStore = writable(themestyle)