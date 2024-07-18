<template>
    <div id="app">
        <h1>Meteorite Data</h1>
        <MeteoriteTable />
    </div>
</template>

<script>
    import MeteoriteTable from './components/MeteoriteTable.vue';

    export default {
        name: 'App',
        components: {
            MeteoriteTable
        }
    };
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
