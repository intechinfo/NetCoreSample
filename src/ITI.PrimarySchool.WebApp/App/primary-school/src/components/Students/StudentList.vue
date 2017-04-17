<template>
    <div>
        <div class="page-header">
            <h1>Gestion des élèves</h1>
        </div>

        <div class="panel panel-default">
            <div class="panel-body text-right">
                <router-link class="btn btn-primary" :to="`students/create`"><i class="glyphicon glyphicon-plus"></i> Ajouter un élève</router-link>
            </div>
        </div>

        <table class="table table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Nom</th>
                    <th>Prénom</th>
                    <th>Date de naissance</th>
                    <th>Login github</th>
                    <th>Options</th>
                </tr>
            </thead>

            <tbody>
                <tr v-if="studentList.length == 0">
                    <td colspan="6" class="text-center">Il n'y a actuellement aucun élève.</td>
                </tr>

                <tr v-for="i of studentList">
                    <td>{{ i.studentId }}</td>
                    <td>{{ i.lastName }}</td>
                    <td>{{ i.firstName }}</td>
                    <td>{{ i.birthDate }}</td>
                    <td>{{ i.gitHubLogin }}</td>
                    <td>
                        <router-link :to="`students/edit/${i.studentId}`"><i class="glyphicon glyphicon-pencil"></i></router-link>
                        <a href="#" @click="deleteStudent(i.studentId)"><i class="glyphicon glyphicon-remove"></i></a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    import { mapActions } from 'vuex'
    import StudentApiService from '../../services/StudentApiService'

    export default {
        data() {
            return {
                studentList: []
            }
        },

        async mounted() {
            await this.refreshList();
        },

        methods: {
            ...mapActions(['tryExecuteAsyncRequest', 'executeAsyncRequest']),

            async refreshList() {
                this.studentList = await this.tryExecuteAsyncRequest(() => StudentApiService.getStudentListAsync());
            },

            async deleteStudent(studentId) {
                try {
                    await this.executeAsyncRequest(() => StudentApiService.deleteStudentAsync(studentId));
                    await this.refreshList();
                }
                catch(error) {

                }
            }
        }
    }
</script>

<style lang="less">

</style>