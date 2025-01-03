export type recievedMessage = {
    Sender: string;
    Content: string;
    DocumentId: string;
  }
export type Message = {
    Sender: string;
    Content: string;
  }
export type SearchedUser = {
    id: string
    username: string
}
export type GroupStore = {
    users: string[]
    isCreatingGroup: boolean
}
export type MessageDataToStore = {
    Sender: string;
    Content: string;
    Recipient: string;
}
export type GroupChatData = {
    RoomName: string
    UsersToAdd: string[]
    IsPublic: boolean
}
export type DirectMessage = {
    DocumentId: string
    Recipient: string
    MessagesReceived: number
}
export type GroupChat = {
    DocumentId: string
    RoomName: string
    MessagesReceived: number
}
export type LoginData = {
    password: string,
}
export type User = {
    username: string 
    directMessageDocuments: DirectMessage[] 
    groupChatDocuments: GroupChat[] 
    isLoggedIn: boolean
    currentRecipient: string
}