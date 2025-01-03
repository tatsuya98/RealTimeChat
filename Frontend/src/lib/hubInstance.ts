import * as signalR from "@microsoft/signalr";
import type {  GroupChat, recievedMessage, User } from "../Utils/customTypes";
import { userStore } from "./userStore";

let user: User ;
userStore.subscribe((value) => {
  user = value;
});

interface HubCallbacks {
    onReceiveMessage: (messageData: recievedMessage) => void;
    onSendMessageConfirmation: (messageData: recievedMessage) => void;
    onReceivePrivateMessage: (messageData: recievedMessage) => void;
    onGroupChatCreated: (groupChatData: GroupChat) => void;
    onReceiveGroupChatMessage: (messageData: recievedMessage) => void;
  }
  
  let callbacks: HubCallbacks = {
    onReceiveMessage: () => { },
    onSendMessageConfirmation: () => { },
    onReceivePrivateMessage: () => { },
    onGroupChatCreated: () => { },
    onReceiveGroupChatMessage: () => {}
  };

export const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7079/hub")
      .build();
      connection.on("ReceiveMessage", (messageData: recievedMessage) => {
        callbacks.onReceiveMessage(messageData);
      });
      connection.on("SendMessageConfirmation", (messageData: recievedMessage) => {
        callbacks.onSendMessageConfirmation(messageData);
      });
      connection.on("ReceivePrivateMessage", (messageData: recievedMessage) => {
        
          callbacks.onReceivePrivateMessage(messageData);
      });
      connection.on("GroupChatCreated", (groupChatData: GroupChat) => {
        callbacks.onGroupChatCreated(groupChatData);
      })
      connection.on("ReceiveGroupMessage", (messageData: recievedMessage) => {
        callbacks.onReceiveGroupChatMessage(messageData);
      })
        await connection.start().catch((err) => console.log(err));
      export const setCallbacks = (newCallbacks: HubCallbacks) => {
        callbacks = newCallbacks;
      };