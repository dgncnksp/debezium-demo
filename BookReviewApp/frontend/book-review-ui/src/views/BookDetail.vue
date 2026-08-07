<script setup>
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import api from "../api/api";

const route = useRoute();

const book = ref(null);

const loadBook = async () => {

    const response = await api.get(`/books/${route.params.id}`);

    book.value = response.data;
}

onMounted(loadBook);
</script>

<template>

<div
    v-if="book"
    class="container"
>

<h1>{{ book.title }}</h1>

<h3>{{ book.author }}</h3>

<p>{{ book.publishedYear }}</p>

<p>

{{ book.description }}

</p>

<router-link to="/">
← Listeye Dön
</router-link>

</div>

</template>