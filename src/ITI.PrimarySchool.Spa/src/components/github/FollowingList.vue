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
    import { getFollowingList } from '../../api/githubApi'

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
            async loadList() {
                try {
                    this.following = await getFollowingList();
                }
                catch(e) {
                    console.error(e);
                }
            }
        }
    }
</script>

<style lang="scss">

</style>