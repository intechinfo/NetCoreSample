<template>
    <div>
        <div class="page-header">
            <h1>Assigner une classe à un professeur</h1>
        </div>

        <form @submit="onSubmit($event)" v-if="model">
            <div class="form-group">
                <label class="required">Classe assignée : </label>
                <select class="form-control" v-model="model.classId">
                    <option :value="0">Aucune classe</option>
                    
                    <option v-for="c of availableClasses" :value="c.classId" >
                        {{ c.name }}
                    </option>
                </select>
            </div>

            <button type="submit" class="btn btn-primary">Assigner</button>
        </form>
    </div>
</template>

<script>
    import { mapGetters, mapActions } from 'vuex'
    import TeacherApiService from '../../services/TeacherApiService'
    import ClassApiService from '../../services/ClassApiService'

    export default {
        data () {
            return {
                model: null,
                availableClasses: []
            }
        },

        mounted() {
            const teacherId = this.$route.params.id;

            this.loadModel(teacherId);
        },

        methods: {
            ...mapActions(['requestAsync']),

            redirectToList: function() {
                this.$router.replace('/teachers');
            },

            loadModel: async function(teacherId) {
                try {
                    this.model = await this.requestAsync(() => TeacherApiService.getTeacherAssignedClassAsync(teacherId), true);
                    this.model.teacherId = teacherId;
                    this.availableClasses = await this.requestAsync(() => ClassApiService.getClassesWithoutTeacherAsync(teacherId), true);

                    if(this.model.classId > 0) this.availableClasses.push(this.model);
                }
                catch(e) {
                    return this.redirectToList();
                }
            },

            saveModel: async function() {
                try {
                    await this.requestAsync(() => TeacherApiService.assignTeacherToclassAsync(this.model.teacherId, this.model.classId), true);
                    this.redirectToList();
                }
                catch(e) {
                    // Custom error management here.
                    // In our application, errors throwed when executing a request are managed globally directly in the actions: they are added to the 'app.errors' state.
                    // A custom component should react to this state when a new error is added, and make an action, like showing an alert message, or something else.
                    // By the way, you can handle errors manually for each component if you want it...
                }
            },

            onSubmit: function(e) {
                e.preventDefault();

                this.saveModel();
            }
        }
    }
</script>

<style lang="less">

</style>