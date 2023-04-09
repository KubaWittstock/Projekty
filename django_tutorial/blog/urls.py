from django.urls import path
from . import views

urlpatterns = [
    path('', views.home, name="home"),
    path('posts/', views.all_posts, name="all_posts"),
    path('posts/<slug:post_slug>', views.post, name="post"),
    path('tags/<slug:tag>', views.posts_with_tag, name="tag"),
]
