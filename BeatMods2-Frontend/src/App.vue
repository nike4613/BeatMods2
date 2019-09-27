<template>
    <div id="app">
        <header id="header" class="Header">
            <div class="Header-item--full">
                <router-link to="/" class="Header-link h4"
                    >BeatMods 2</router-link
                >
            </div>
            <div class="Header-item">
                <router-link
                    to="/login"
                    class="Header-link border border-gray-dark px-2 py-1 rounded-1"
                    >Sign in</router-link
                >
            </div>
        </header>
        <nav id="nav" class="SideNav border">
            <router-link to="/" class="SideNav-item" exact>Home</router-link>
            <router-link to="/about" class="SideNav-item" exact
                >About</router-link
            >
        </nav>
        <div id="contents">
            <router-view />
        </div>
    </div>
</template>

<style lang="scss">
@import "@/styles";
</style>
