<template>
    <div id="loginComplete">
        query params:
        <div v-for="(value, name) in $route.query" :key="name">
            <pre><code>{{ name }}: {{ JSON.stringify(value) }}</code></pre>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component({})
export default class LoginComplete extends Vue {}
</script>

<style lang="scss" scoped></style>
