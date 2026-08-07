import { createRouter, createWebHistory } from "vue-router";

import Home from "../views/Home.vue";
import BookDetail from "../views/BookDetail.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: "/",
            name: "home",
            component: Home
        },
        {
            path: "/books/:id",
            name: "book-detail",
            component: BookDetail,
            props: true
        }
    ]
});

export default router;