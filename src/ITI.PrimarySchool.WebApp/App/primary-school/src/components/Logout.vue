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
        created() {
            AuthService.registerSignedOutCallback(this.onSignedOut);
        },

        beforeDestroy() {
            AuthService.removeSignedOutCallback(this.onSignedOut);
        },

        computed: {
            logoutEndpoint: () => AuthService.logoutEndpoint
        },

        methods: {
            onSignedOut() {
                this.$router.replace('/');
            }
        }
    }
</script>

<style lang="less">

</style>