# Generated by Django 4.2.5 on 2023-10-06 12:14

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('core_playlist', '0001_initial'),
    ]

    operations = [
        migrations.AlterField(
            model_name='playlist',
            name='description',
            field=models.CharField(null=True),
        ),
        migrations.AlterField(
            model_name='playlist',
            name='title',
            field=models.TextField(default='title music'),
            preserve_default=False,
        ),
    ]
