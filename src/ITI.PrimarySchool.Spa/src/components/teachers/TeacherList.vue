<template>
    <div>
        <div class="mb-4 d-flex justify-content-between">
            <h1>Gestion des professeurs</h1>

            <div>
                <router-link class="btn btn-primary" :to="`teachers/create`"><i class="fa fa-plus"></i> Ajouter un professeur</router-link>
            </div>
        </div>

        <table class="table table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Nom</th>
                    <th>Prénom</th>
                    <th>Options</th>
                </tr>
            </thead>

            <tbody>
                <tr v-if="teacherList.length == 0">
                    <td colspan="4" class="text-center">Il n'y a actuellement aucun professeur.</td>
                </tr>

                <tr v-for="i of teacherList">
                    <td>{{ i.teacherId }}</td>
                    <td>{{ i.lastName }}</td>
                    <td>{{ i.firstName }}</td>
                    <td>
                        <router-link :to="`teachers/edit/${i.teacherId}`"><i class="fa fa-pencil"></i></router-link>
                        <router-link :to="`teachers/assign/${i.teacherId}`"><i class="fa fa-link"></i></router-link>
                        <a href="#" @click="deleteTeacher(i.teacherId)"><i class="fa fa-trash"></i></a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import { getTeacherListAsync, deleteTeacherAsync } from '../../api/teacherApi'

    export default {
        data() {
            return {
                teacherList: []
            }
        },

        async mounted() {
            await this.refreshList();
        },

        methods: {
            async refreshList() {
                try {
                    this.teacherList = await getTeacherListAsync();
                }
                catch(e) {
                    console.error(e);
                }
            },

            async deleteTeacher(teacherId) {
                try {
                    await deleteTeacherAsync(teacherId);
                    await this.refreshList();
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