<template>
    <div>
        <div class="page-header">
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

        mounted() {
            this.loadList();
        },

        methods: {
            ...mapActions(['requestAsync']),

            loadList: async function() {
                var following = await this.requestAsync(() => GitHubApiService.getFollowingList());

                this.following = following;
            }
        }
    }
</script>

<style lang="less">

</style>