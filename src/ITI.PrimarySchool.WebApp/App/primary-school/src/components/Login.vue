<template>
    <div class="text-center">
        <div class="page-header">
            <h1>Bienvenue sur ITI.PrimarySchool</h1>
        </div>

        <button type="button" @click="login" class="btn btn-lg btn-primary">Se connecter</button>
    </div>
</template>

<script>
    import AuthService from '../services/AuthService'

    export default {
        methods: {
            login() {
                AuthService.login();
            }
        }
    }
</script>

<style lang="less">

</style>