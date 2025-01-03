import { fail } from "@sveltejs/kit";
import type { User } from "../../Utils/customTypes";
import type { Actions } from "./$types";

export const actions = {
    default: async ({ request}) => {
          const formData = await request.formData();
          const username = formData.get("username");
          const password = formData.get("password");
          const loginData = {
            password
          }
          const response = await fetch(
              `https://localhost:7079/api/users/${username}`,
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(loginData),
          });
          const userData = await response.json()
          if(!response.ok){
           return fail(response.status, {...userData})
          }
          const userDataObject: User = { ...userData, isLoggedIn: true, currentRecipient: ""};
          return { userDataObject};
    }
} satisfies Actions;
