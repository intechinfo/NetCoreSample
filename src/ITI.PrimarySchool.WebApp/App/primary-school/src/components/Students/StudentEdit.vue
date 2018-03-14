<template>
    <div>
        <div class="mb-4">
            <h1 v-if="mode == 'create'">Créer un élève</h1>
            <h1 v-else>Editer un élève</h1>
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

            <div class="form-group">
                <label class="required">Date de naissance</label>
                <input type="date" v-model="item.birthDate" class="form-control" required>
            </div>

            <div class="form-group">
                <label>Login GitHub</label>
                <input type="text" v-model="item.gitHubLogin" class="form-control">
            </div>

            <button type="submit" class="btn btn-primary">Sauvegarder</button>
        </form>
    </div>
</template>

<script>
    import { mapActions } from 'vuex'
    import StudentApiService from '../../services/StudentApiService'
    import { DateTime } from 'luxon'

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
                    // Here, we use "executeAsyncRequest" action. When an exception is thrown, it is not catched: you have to catch it.
                    // It is useful when we have to know if an error occurred, in order to adapt the user experience.
                    const item = await this.executeAsyncRequest(() => StudentApiService.getStudentAsync(this.id));

                    // Here we transform the date, because the HTML date input expect format "yyyy-MM-dd"
                    item.birthDate = DateTime.fromISO(item.birthDate).toISODate();

                    this.item = item;
                }
                catch(error) {
                    console.error(error);

                    // So if an exception occurred, we redirect the user to the students list.
                    this.$router.replace('/students');
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
                if(!this.item.birthDate) errors.push("Date de naissance")

                this.errors = errors;

                if(errors.length == 0) {
                    try {
                        if(this.mode == 'create') {
                            await this.executeAsyncRequest(() => StudentApiService.createStudentAsync(this.item));
                        }
                        else {
                            await this.executeAsyncRequest(() => StudentApiService.updateStudentAsync(this.item));
                        }

                        this.$router.replace('/students');
                    }
                    catch(error) {
                        // Custom error management here.
                        // In our application, errors throwed when executing a request are managed globally via the "executeAsyncRequest" action: errors are added to the 'app.errors' state.
                        // A custom component should react to this state when a new error is added, and make an action, like showing an alert message, or something else.
                        // By the way, you can handle errors manually for each component if you need it...
                    }
                }
            }
        }
    }
</script>

<style lang="scss">

</style>