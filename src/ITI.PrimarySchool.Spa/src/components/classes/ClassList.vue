<template>
    <div>
        <div class="mb-4 d-flex justify-content-between">
            <h1>Gestion des classes</h1>

            <div>
                <router-link class="btn btn-primary" :to="`classes/create`">
                    <i class="fa fa-plus"></i> Ajouter une classe</router-link>
            </div>
        </div>

        <table class="table table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Nom</th>
                    <th>Niveau</th>
                    <th>Options</th>
                </tr>
            </thead>

            <tbody>
                <tr v-if="classList.length == 0">
                    <td colspan="4" class="text-center">Il n'y a actuellement aucune classe.</td>
                </tr>

                <tr v-for="i of classList" :key="i.classId">
                    <td>{{ i.classId }}</td>
                    <td>{{ i.name }}</td>
                    <td>{{ i.level }}</td>
                    <td>
                        <router-link :to="`classes/edit/${i.classId}`">
                            <i class="fa fa-pencil"></i>
                        </router-link>
                        <a href="#" @click="deleteClass(i.classId)">
                            <i class="fa fa-trash"></i>
                        </a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import { getClassListAsync, deleteClassAsync } from '../../api/classApi'
import { state } from "../../state"

export default {
    data() {
        return {
            classList: []
        }
    },

    async mounted() {
        await this.refreshList();
    },

    methods: {
        async refreshList() {
            try {
                state.isLoading = true;
                this.classList = await getClassListAsync();
            }
            catch (e) {
                console.error(e);
                state.exceptions.push(e);
            }
            finally {
                state.isLoading = false;
            }
        },

        async deleteClass(classId) {
            try {
                state.isLoading = true;

                await deleteClassAsync(classId);
                await this.refreshList();
            }
            catch (e) {
                console.error(e);
                state.exceptions.push(e);
            }
            finally {
                state.isLoading = false;
            }
        }
    }
}
</script>

<style lang="scss">

</style>