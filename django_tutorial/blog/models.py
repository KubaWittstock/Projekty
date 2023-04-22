from django.db import models
from django.core.validators import MinLengthValidator

# Create your models here.

class Image(models.Model):
    dir = models.CharField(max_length=100)
    alt = models.CharField(max_length=100)
    
    def __str__(self) -> str:
        return self.alt

class Tag(models.Model):
    display_name = models.CharField(max_length=50, unique=True)
    slug = models.SlugField(unique=True)

    def __str__(self) -> str:
        return self.display_name


class Author(models.Model):
    first_name = models.CharField(max_length=50)
    last_name = models.CharField(max_length=50)
    email = models.EmailField()

    def __str__(self) -> str:
        return f"{self.first_name} {self.last_name}"

class Post(models.Model):
    slug = models.SlugField(unique=True, max_length=100)
    title = models.CharField(max_length=100)
    content = models.TextField(validators=[MinLengthValidator(10)])
    summary = models.CharField(max_length=255, blank=True, null=True)
    date_created = models.DateField()
    last_modified = models.DateField(auto_now=True)
    tags = models.ManyToManyField(Tag)
    cover_img = models.OneToOneField(Image, on_delete=models.CASCADE)
    author = models.ForeignKey(Author, on_delete=models.SET_NULL, related_name="posts", null=True)

    def __str__(self) -> str:
        return f"Post #{self.id}: {self.title}, created: {self.date_created}"
    
    class Meta:
        get_latest_by="date_created"

