from django.contrib import admin
from .models import Post, Tag, Image, Author


class PostAdmin(admin.ModelAdmin):
    list_filter = ("date_created", 'author', 'tags')
    list_display = ("title", "date_created", 'author', 'last_modified',)


class AuthorAdmin(admin.ModelAdmin):
    list_filter = ("first_name", 'last_name',)
    list_display = ("first_name", 'last_name',)


class TagAmdmin(admin.ModelAdmin):
    prepopulated_fields = {"slug": ("display_name", )}
    list_display = ("display_name",)


# Register your models here.
admin.site.register(Post, PostAdmin)
admin.site.register(Tag, TagAmdmin)
admin.site.register(Author, AuthorAdmin)
admin.site.register(Image)
