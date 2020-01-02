<template>
    <div id="loginComplete">
        <h1 v-if="isFailed">Operation failed</h1>
        <div v-if="isDone">
            <h1>Operation complete</h1>
            <h2 v-if="newUser">New user created</h2>
            <pre>{{ token }}</pre>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { authenticate } from "../../api/users/authenticate";

@Component({})
export default class Complete extends Vue {
    mounted() {
        const success_name = "successful";

        let query = this.$route.query;
        let successful =
            success_name in query
                ? (query[success_name] as string) === "true"
                : true;
        if (!successful) {
            // TODO: do something
            this.isFailed = true;
        } else {
            let code = query["code"] as string;

            authenticate(code).then((r) => {
                // TODO: add error checks
                this.isDone = true;
                this.newUser = r.isNewUser;
                this.token = r.token;
            });
        }
    }

    isFailed: boolean = false;
    isDone: boolean = false;
    newUser: boolean = false;
    token: string = "";
}
</script>

<style lang="scss" scoped></style>
