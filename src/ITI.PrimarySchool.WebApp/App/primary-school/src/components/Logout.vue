<template>
    <div class="text-center">
        <i class="fa fa-refresh fa-spin fa-3x fa-fw"></i>
        <br />
        <h1>Déconnexion en cours...</h1>
        
        <iframe :src="logoutEndpoint" frameborder="0" width="0" height="0"></iframe>
    </div>
</template>

<script>
    import AuthService from '../services/AuthService'

    export default {
        computed: {
            logoutEndpoint: () => AuthService.logoutEndpoint
        }
    }
</script>

<style lang="less">

</style>