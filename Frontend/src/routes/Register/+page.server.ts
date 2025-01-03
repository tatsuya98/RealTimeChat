import { fail} from "@sveltejs/kit";
import type { User } from "../../Utils/customTypes";
import type { Actions } from "./$types";
import { passwordMatchChek } from "../../Utils/utils";

export const actions = {
    default: async( {request}) => {
        const formData = await request.formData();
        const username = formData.get("username");
        const password = formData.get("password");
        const confirmPassword = formData.get("confirmPassword");
        if(!passwordMatchChek(password, confirmPassword)){
            return fail(400, {value:{message: "Passwords don't match"}});
        }
        const response = await fetch("https://localhost:7079/api/users", {
            method: "POST",
            headers: {
              "content-type": "application/json",
            },
            body: JSON.stringify({ username, password }),
          });
          const data = await response.json();
          if(!response.ok){
            return fail(response.status, {...data})
          }
          const userDataObject: User = { ...data, isLoggedIn: true, currentRecipient: "" };
        return { userDataObject };
    }
} satisfies Actions;