import Vue from "vue";
import Router from "vue-router";
import Home from "./views/Home.vue";
import Login from "./views/LogIn/LogIn.vue";
import LoginComplete from "./views/LogIn/Complete.vue";
import NotFound from "./views/NotFound.vue";
import QueryDebug from "./views/QueryDebug.vue";

Vue.use(Router);

export default new Router({
    mode: "history",
    base: process.env.BASE_URL,
    routes: [
        {
            path: "/",
            name: "home",
            component: Home,
        },
        {
            path: "/mods",
            name: "mods",
            component: () =>
                import(/* webpackChunkName: "mods" */ "./views/Mods.vue"),
        },
        {
            path: "/login",
            name: "login",
            component: Login,
        },
        {
            path: "/login/complete",
            name: "login complete",
            component: LoginComplete,
        },
        {
            path: "*",
            name: "404",
            component: NotFound,
        },
    ],
});
