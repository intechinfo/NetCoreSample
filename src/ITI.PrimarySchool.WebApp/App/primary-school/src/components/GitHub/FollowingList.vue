<template>
    <div>
        <div class="mb-4 d-flex justify-content-between">
            <h1>Elèves suivis sur GitHub</h1>
        </div>

        <table class="table table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Nom</th>
                    <th>Prénom</th>
                    <th>Compte GitHub</th>
                </tr>
            </thead>

            <tbody>
                <tr v-if="following.length == 0">
                    <td colspan="4" class="text-center">Vous ne suivez actuellement aucun élève sur GitHub.</td>
                </tr>

                <tr v-for="i of following">
                    <td>{{ i.studentId }}</td>
                    <td>{{ i.lastName }}</td>
                    <td>{{ i.firstName }}</td>
                    <td>{{ i.gitHubLogin }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import GitHubApiService from '../../services/GitHubApiService'

    export default {
        data() {
            return {
                following: []
            }
        },

        async mounted() {
            await this.loadList();
        },

        methods: {
            ...mapActions(['executeAsyncRequestOrDefault']),

            async loadList() {
                // We use the "executeAsyncRequestOrDefault" (cf. vuex/actions.js) action that does all the job for us :
                // 1) Set the loading state to true
                // 2) Execute the callback
                // 3) In case of error, notify the application of that
                // 4) Reset the loading state to false
                // The callback is a "lambda function", like in C#
                // "executeAsyncRequestOrDefault" is nice: if an exception is thrown during the request, it is automatically catched, and the return value is undefined... 
                // So, it fails silently.
                this.following = (await this.executeAsyncRequestOrDefault(() => GitHubApiService.getFollowingList())) || [];
            }
        }
    }
</script>

<style lang="scss">

</style>