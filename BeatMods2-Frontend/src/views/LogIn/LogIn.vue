<template>
    <div id="login" class="pl-12 pr-12 pt-8 pb-8">
        <h1>Log In</h1>
        <hr />
        <a
            :href="loginLink"
            id="gh-login"
            class="border border-gray-dark px-2 py-1 rounded-1"
            ><Octicon iconName="mark-github" />Log in with GitHub</a
        >
    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import Octicon from "../../components/Octicon.vue";
import { getGithubAuthUrl } from "../../api/users/login";

@Component({
    components: {
        Octicon,
    },
})
export default class LogIn extends Vue {
    mounted() {
        getGithubAuthUrl({
            success: location.origin + "/login/complete",
        }).then((s) => (this.loginLink = s));
    }

    loginLink: string = "";
}
</script>

<style lang="scss" scoped>
#gh-login {
    font-size: 1.2em;
    color: black;
}
</style>
