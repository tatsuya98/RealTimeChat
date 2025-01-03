

import { goto } from "$app/navigation";
import { themeStore } from "$lib/ThemeStore";
import { userStore } from "$lib/userStore";
import { utilStore } from "$lib/UtilStore";
import isOnline from "is-online";
import type { User, DirectMessage, GroupChat, Message, recievedMessage, GroupStore } from "./customTypes";
import { connection } from "$lib/hubInstance";
import type { Writable } from "svelte/store";

export function convertUnknownDataToUserData(data: unknown): User  {
    
    const userData: User = {
      username: (data as { username: string }).username,
      directMessageDocuments: (data as {directMessageDocuments: DirectMessage[]}).directMessageDocuments,
      groupChatDocuments: (data as {groupChatDocuments: GroupChat[]}).groupChatDocuments,
      isLoggedIn:( data as { isLoggedIn: boolean }).isLoggedIn,
      currentRecipient: (data as {currentRecipient: string}).currentRecipient,
    }
    return userData
}
export function convertUnknownErrorMessageToString(error: unknown): string {
  return (error as { message: string }).message
}

export function hideError(errorMessage: string | null ): void {
  errorMessage = null;
}
export function passwordMatchChek(
  password: FormDataEntryValue | null,
  confirmPassword: FormDataEntryValue | null
): boolean {
  return password === confirmPassword;
}

export function generateRandomString(length: number): string {
  const characters =
    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  let result = "";
  for (let i = 0; i < length; i++) {
    result += characters.charAt(
      Math.floor(Math.random() * characters.length)
    );
  }
  return result;
}

export function updateGeneralMessages(messages: Message[], messageData: recievedMessage): Message[] {
  messages = [...messages, { ...messageData }];
  return messages
}

export  function handlePrivateMessage(messages: Message[], messageData: recievedMessage, DocumentId: string, isDirectMessage: boolean, user: User): Message[] {
  
  let MessagesReceived = user.directMessageDocuments?.find((doc) => doc.DocumentId === messageData.DocumentId)?.MessagesReceived;
  if (isDirectMessage) {
    messages = [...messages, { ...messageData }];
  } else {
    if (userDocumentExists(user, "directMessage" ,messageData.DocumentId)) {
        incrementUnreadMessages(MessagesReceived!, "directMessage", messageData.DocumentId);
        return []
    } else {
      userStore.update((user) => {
        const tempUserDocs: DirectMessage = {
          DocumentId: messageData.DocumentId,
          Recipient: messageData.Sender,
          MessagesReceived: MessagesReceived! + 1
        };
        user!.directMessageDocuments = [
          ...(user!.directMessageDocuments ?? []),
          tempUserDocs,
        ];
        return user;
      });
       connection.invoke("UpdateMessagesReceived", user.username, messageData.DocumentId);
      return []
    }
  }
  if (DocumentId !== messageData.DocumentId)
    DocumentId = messageData.DocumentId;
  return messages
}

export function handleGroupMessage(messages: Message[], messageData: recievedMessage, DocumentId: string, isGroupChat: boolean, user: User): Message[] {
  let MessagesReceived = user.directMessageDocuments?.find((doc) => doc.DocumentId === messageData.DocumentId)?.MessagesReceived;
  if(isGroupChat){
    messages = [...messages, { ...messageData }];
  }else{
    if(userDocumentExists(user, "groupChat" ,messageData.DocumentId)) {
      incrementUnreadMessages(MessagesReceived!,"groupChat", DocumentId );
    }
  }
  return messages
}

export function userDocumentExists( user: User, documentType: string ,DocumentId: string): boolean {
  if(documentType === "directMessage") return user.directMessageDocuments!.some((doc) => doc.DocumentId === DocumentId)
  return user.groupChatDocuments!.some((doc) => doc.DocumentId === DocumentId)
}

export function handleButtonStyleChange(e: Event, isDarkMode: boolean, lightThemeColor: string, darkThemeColor: string): void {
  let target = e.target as HTMLButtonElement;
  target.style.cursor = "pointer";
  if (isDarkMode && target instanceof HTMLButtonElement) {
    target.style.backgroundColor = lightThemeColor;
  } else {
    target.style.backgroundColor = darkThemeColor;
  }
}

export  function handleButtonStyleRevert(e: Event, isDarkMode: boolean, lightThemeColor: string, darkThemeColor: string): void {
  let target = e.target as HTMLButtonElement;
  if(isDarkMode){
    target.style.backgroundColor = darkThemeColor
  }else{
    target.style.backgroundColor = lightThemeColor;
  }
  
}

 export function handleAnchorStyleChange(e: Event, isDarkMode: boolean, lightThemeColor: string, darkThemeColor: string): void {
  let target = e.target as HTMLAnchorElement;

  if (isDarkMode && target instanceof HTMLAnchorElement) {
    target.style.color = lightThemeColor;
  } else {
    target.style.color = darkThemeColor;
  }
}
export function handleRevert(e: Event): void {
  let target = e.target as HTMLElement;
  target.style.color = "inherit";
}

export function toggleMenu(open:boolean): void {
  open = !open;
  utilStore.update((state: { isOpen: boolean; showProfileSidebar: boolean, showSearchbar:boolean;showChatSidebar: boolean, isConnected: boolean, isSmallScreen: boolean }) => {
    state.isOpen = open;
    return state;
  });
}

export function Logout() {
  const resetUserData: User = {
    username: "",
    directMessageDocuments: [],
    groupChatDocuments: [],
    isLoggedIn: false,
    currentRecipient: "",
  };
  userStore.set(resetUserData);
  goto("/");
}

export function toggleDarkMode(isDarkMode: boolean, lightThemeColor: string, darkThemeColor: string, lightBackgroundColor: string, darkBackgroundColor: string): void {
  isDarkMode = !isDarkMode;
  const messageWindow = document.querySelector(".messages") as HTMLElement;
  
  if(isDarkMode && messageWindow !== null){
    window.document.body.classList.add("dark-mode");
    messageWindow.style.backgroundColor = darkBackgroundColor;
  }
  if(!isDarkMode && messageWindow !== null){
    window.document.body.classList.remove("dark-mode");
    messageWindow.style.backgroundColor = lightBackgroundColor;
  }
  if(isDarkMode && messageWindow === null){
    window.document.body.classList.add("dark-mode");
  }else if (!isDarkMode && messageWindow === null){
    window.document.body.classList.remove("dark-mode");
  }
  window.localStorage.setItem("darkMode", JSON.stringify(isDarkMode));
  themeStore.update((state) => {
    state.isDarkMode = isDarkMode;
    return state;
  });
}
export function toggleDarkModeButtons(isDarkMode: boolean, lightThemeColor: string, darkThemeColor: string): void {
  const buttons = document.getElementsByTagName("button");
  isDarkMode = !isDarkMode;
  for (let i = 0; i < buttons.length; i++) {
    const button = buttons[i] as HTMLElement;
    button.style.backgroundColor = darkThemeColor;
    if (!isDarkMode) {
      button.style.backgroundColor = lightThemeColor;
    }
  }
}
export function toggleSidebar(showProfileSidebar: boolean | null, showChatSidebar: boolean | null): void {
  if(showProfileSidebar !== null){
    utilStore.update((state: { isOpen: boolean; showProfileSidebar: boolean, showSearchbar:boolean;showChatSidebar: boolean, isConnected: boolean, isSmallScreen: boolean }) => {
      state.showProfileSidebar = !showProfileSidebar;
      return state;
    });
  }else if (showChatSidebar !== null){
    utilStore.update((state: { isOpen: boolean; showProfileSidebar: boolean, showSearchbar:boolean;showChatSidebar: boolean, isConnected: boolean, isSmallScreen: boolean }) => {
      state.showChatSidebar = !showChatSidebar;
      return state;
    });
  }
}

export async function reconnectUser(user: User): Promise<void> {
  try{
    if(await isOnline()){
      utilStore.update((state: { isOpen: boolean; showProfileSidebar: boolean, showSearchbar:boolean;showChatSidebar: boolean, isConnected: boolean, isSmallScreen: boolean }) => {
        state.isConnected = true;
        return state;
      });
      await connection.start();
      if(user.isLoggedIn){
        await connection.invoke("UpdateReconnectedUser", user.username);
      }
    }else{
      const errorMessage = "You are currently offline, please check your internet connection to reconnect.";
      const errorH1 = document.createElement("h1");
      errorH1.textContent = errorMessage;
      errorH1.style.color = "red";
      document.querySelector("body")?.appendChild(errorH1);
    }
  }catch (error) {
    console.log(error);
  }
}

export function incrementUnreadMessages(MessagesReceived: number, documentType: string, documentId: string): number {
  MessagesReceived += 1;
  userStore.update((user) => {
   if(documentType === "directMessage"){
    user.directMessageDocuments.map((doc) => {
      if(doc.DocumentId === documentId){
        doc.MessagesReceived = MessagesReceived;
      }
      
      connection.invoke("UpdateMessagesReceived", user.username, documentId, "directMessage");
    })
   }else {
    user.groupChatDocuments.map((doc) => {
      if(doc.DocumentId === documentId){
        doc.MessagesReceived = MessagesReceived;
      }
    })
    connection.invoke("UpdateMessagesReceived", user.username, documentId, "groupChat");
  }
    return user
  })
  return MessagesReceived;
}

export function resetMessagesReceived(documentId: string, username: string, documentType: string, messagesReceived: number): void {
  if(messagesReceived > 0){
    userStore.update((user) => {
      user.directMessageDocuments.map((doc) => {
        if(doc.DocumentId === documentId){
          doc.MessagesReceived = 0;
        }
      })
      return user;
    })
    connection.invoke("ResetMessagesReceived", documentId, username, documentType);
  }
}
export function addUserToGroupChat(groupChatStore: Writable<GroupStore>, user: User): void {
  groupChatStore.update(
    (state: { users: string[]; isCreatingGroup: boolean }) => {
      state.users.push(user.username);
      return state;
    }
  );
  goto("/CreateGroupChat");
}

export function toggleSearchbar(showSearchbar: boolean): void {
  utilStore.update((state: { isOpen: boolean; showProfileSidebar: boolean, showSearchbar:boolean;showChatSidebar: boolean, isConnected: boolean, isSmallScreen: boolean }) => {
    state.showSearchbar = !showSearchbar;
    return state;
  });
}