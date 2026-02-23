<template>
  <Transition
    enter-active-class="transition-opacity duration-500"
    enter-from-class="opacity-0"
    enter-to-class="opacity-100"
    leave-active-class="transition-opacity duration-500"
    leave-from-class="opacity-100"
    leave-to-class="opacity-0"
  >
    <div
      v-if="visible"
      class="bg-red-100 border border-red-400 text-red-700 px-4 py-2 rounded mb-4"
    >
      {{ message }}
    </div>
  </Transition>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';

const props = defineProps<{ message: string | null }>();
const visible = ref(false);

watch(
  () => props.message,
  (val) => {
    if (val) {
      visible.value = true;
      setTimeout(() => {
        visible.value = false;
      }, 5000); // auto-hide after 5s
    } else {
      visible.value = false;
    }
  }
);
</script>
