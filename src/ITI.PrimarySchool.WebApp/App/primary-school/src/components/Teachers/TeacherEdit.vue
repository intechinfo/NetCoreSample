<template>
    <div>
        <div class="mb-4">
            <h1 v-if="mode == 'create'">Créer un professeur</h1>
            <h1 v-else>Editer un professeur</h1>
        </div>

        <form @submit="onSubmit($event)">
            <div class="alert alert-danger" v-if="errors.length > 0">
                <b>Les champs suivants semblent invalides : </b>

                <ul>
                    <li v-for="e of errors">{{e}}</li>
                </ul>
            </div>

            <div class="form-group">
                <label class="required">Nom</label>
                <input type="text" v-model="item.lastName" class="form-control" required>
            </div>

            <div class="form-group">
                <label class="required">Prénom</label>
                <input type="text" v-model="item.firstName" class="form-control" required>
            </div>

            <button type="submit" class="btn btn-primary">Sauvegarder</button>
        </form>
    </div>
</template>

<script>
    import { mapActions } from 'vuex'
    import TeacherApiService from '../../services/TeacherApiService'

    export default {
        data () {
            return {
                item: {},
                mode: null,
                id: null,
                errors: []
            }
        },

        async mounted() {
            this.mode = this.$route.params.mode;
            this.id = this.$route.params.id;

            if(this.mode == 'edit') {
                try {
                    this.item = await this.executeAsyncRequest(() => TeacherApiService.getTeacherAsync(this.id));
                }
                catch(error) {
                    this.$router.replace('/teachers');
                }
            }
        },

        methods: {
            ...mapActions(['executeAsyncRequest']),

            async onSubmit(e) {
                e.preventDefault();

                var errors = [];

                if(!this.item.lastName) errors.push("Nom")
                if(!this.item.firstName) errors.push("Prénom")

                this.errors = errors;

                if(errors.length == 0) {
                    try {
                        if(this.mode == 'create') {
                            await this.executeAsyncRequest(() => TeacherApiService.createTeacherAsync(this.item));
                        }
                        else {
                            await this.executeAsyncRequest(() => TeacherApiService.updateTeacherAsync(this.item));
                        }

                        this.$router.replace('/teachers');
                    }
                    catch(error) {
                    }
                }
            }
        }
    }
</script>

<style lang="scss">

</style>