<template>
    <div>
        <div class="mb-4">
            <h1 v-if="mode == 'create'">Créer une classe</h1>
            <h1 v-else>Editer une classe</h1>
        </div>

        <form @submit="onSubmit($event)">
            <div class="alert alert-danger" v-if="errors.length > 0">
                <b>Les champs suivants semblent invalides : </b>

                <ul>
                    <li v-for="e of errors">{{e}}</li>
                </ul>
            </div>

            <div class="form-group">
                <label class="required">Nom de la classe</label>
                <input type="text" v-model="item.name" class="form-control" required>
            </div>

            <div class="form-group">
                <label class="required">Niveau</label>
                <select v-model="item.level" class="form-control" required>
                    <option>CP</option>
                    <option>CE1</option>
                    <option>CE2</option>
                    <option>CM1</option>
                    <option>CM2</option>
                </select>
            </div>

            <button type="submit" class="btn btn-primary">Sauvegarder</button>
        </form>
    </div>
</template>

<script>
import { mapActions } from 'vuex'
import ClassApiService from '../../services/ClassApiService'

export default {
    data() {
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

        if (this.mode == 'edit') {
            // Here, we are doing manually many things.
            // In other components (GitHub, Students, Teachers), we use two actions named "executeAsyncRequest" and "executeAsyncRequestOrDefault" that does the job for us.
            try {
                // One: we notify the application that a request will be loading
                this.notifyLoading(true);

                // We call the service to get informations from the server-side api
                this.item = await ClassApiService.getClassAsync(this.id);
            }
            catch (error) {
                // Two: in case of error, we notify the application
                this.notifyError(error);
                this.$router.replace('/classes');
            }
            finally {
                // Three: in all cases, we reset the "loading" state to false. 
                this.notifyLoading(false);
            }
        }
    },

    methods: {
        // Helper from Vuex, injects the named actions (cf. vuex/actions.js) to the methods of the component
        ...mapActions(['notifyLoading', 'notifyError']),

        async onSubmit(e) {
            e.preventDefault();

            // Google Chrome handles form validation based on type of the input, and presence of the "required" attribute.
            // However, it's not (yet) fully supported by all the web browsers.
            // Therefore, the code below handles validation but is very naive: a better validation is desirable.
            var errors = [];

            if (!this.item.name) errors.push("Nom de la classe")
            if (!this.item.level) errors.push("Niveau")

            this.errors = errors;

            if (errors.length == 0) {
                try {
                    this.notifyLoading(true);

                    if (this.mode == 'create') {
                        await ClassApiService.createClassAsync(this.item);
                    }
                    else {
                        await ClassApiService.updateClassAsync(this.item);
                    }

                    this.$router.replace('/classes');
                }
                catch (error) {
                    this.notifyError(error);
                }
                finally {
                    this.notifyLoading(false);
                }
            }
        }
    }
}
</script>

<style lang="scss">

</style>