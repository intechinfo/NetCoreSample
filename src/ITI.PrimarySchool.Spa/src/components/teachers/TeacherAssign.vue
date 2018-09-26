<template>
    <div>
        <div class="mb-4">
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
    import { getTeacherAssignedClassAsync, assignTeacherToclassAsync } from '../../api/teacherApi'
    import { getClassesWithoutTeacherAsync } from '../../api/classApi'

    export default {
        data () {
            return {
                model: null,
                teacherId: null,
                availableClasses: []
            }
        },

        async mounted() {
            this.teacherId = this.$route.params.id;

            try {
                this.model = await getTeacherAssignedClassAsync(this.teacherId);
                this.availableClasses = await getClassesWithoutTeacherAsync(this.teacherId);

                if(this.model.classId > 0) this.availableClasses.push(this.model);
            }
            catch(e) {
                console.error(e);
                return this.redirectToList();
            }
        },

        methods: {
            redirectToList() {
                this.$router.replace('/teachers');
            },

            async onSubmit(event) {
                event.preventDefault();

                try {
                    await assignTeacherToclassAsync(this.teacherId, this.model.classId);
                    this.redirectToList();
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