<script lang="ts">
  import { enhance } from "$app/forms";
  import { goto } from "$app/navigation";
  import { themeStore } from "$lib/ThemeStore";
  import { userStore } from "$lib/userStore";
  import {
    convertUnknownDataToUserData,
    convertUnknownErrorMessageToString,
    handleButtonStyleChange,
    handleButtonStyleRevert,
    hideError,
  } from "../../Utils/utils";

  enum InputType {
    password = "password",
    text = "text",
  }
  let errorMessage: string | null = null;
  let passwordInputType: InputType = InputType.password;
  let isDarkMode: boolean;
  let lightThemeColor = $themeStore.lightTheme["color-400"];
  let darkThemeColor = $themeStore.darkTheme["color-400"];

  $: isDarkMode = $themeStore.isDarkMode;
  function showPassword() {
    if (passwordInputType === InputType.password) {
      passwordInputType = InputType.text;
    } else {
      passwordInputType = InputType.password;
    }
  }
</script>

<div class="form-container">
  {#if errorMessage}
    <p>{errorMessage}</p>
  {/if}
  <h1>Register</h1>
  <form
    class="form-input"
    method="POST"
    use:enhance={() => {
      return async function ({ result }) {
        if (result.type === "success" && result.data) {
          const userData = convertUnknownDataToUserData(
            result.data.userDataObject
          );
          userStore.set(userData);
          goto("/");
        } else if (result.type === "failure" && result.data) {
          errorMessage = convertUnknownErrorMessageToString(result.data.value);
        }
      };
    }}
  >
    <div class="input-container">
      <label for="username">Username</label>
      <input
        on:focus={() => {
          hideError(errorMessage);
        }}
        type="text"
        name="username"
        id="username"
      />
    </div>
    <div class="input-container">
      <label for="password">Password</label>
      <input
        on:focus={() => {
          hideError(errorMessage);
        }}
        type={passwordInputType}
        name="password"
        id="password"
      />
    </div>
    <div class="input-container">
      <label for="confirmPassword">Confirm Password</label>
      <input
        on:focus={() => {
          hideError(errorMessage);
        }}
        type={passwordInputType}
        name="confirmPassword"
        id="confirmPassword"
      />
      <div class="flex-container">
        <input
          type="checkbox"
          name="showPassword"
          id="showPassword"
          on:click={showPassword}
        />
        <label for="showPassword">Show Password</label>
      </div>
    </div>
    <button
      on:mouseover={(e) =>
        handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
      on:focus={(e) =>
        handleButtonStyleChange(e, isDarkMode, lightThemeColor, darkThemeColor)}
      on:blur={(e) =>
        handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
      on:mouseleave={(e) =>
        handleButtonStyleRevert(e, isDarkMode, lightThemeColor, darkThemeColor)}
      type="submit">Register</button
    >
  </form>
</div>
